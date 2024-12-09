import React, { useState } from "react";
import './LoginPage.css';
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";


const SignupPage = () => {
    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [repeatPassword, setRepeatPassword] = useState<string>('');
    const [usernameError, setUsernameError] = useState<string>('');
    const [passwordError, setPasswordError] = useState<string>('');
    const [repeatPasswordError, setRepeatPasswordError] = useState<string>('');
    const navigate = useNavigate();

    const checkLogin = (password: string, repeatPassword: string) => {
        return password === repeatPassword;
    }

    const checkUsernameAvailability = async (username: string) => {
        try {
            const response = await fetch(`https://localhost:44399/api/User/CheckUsernameAvailability?username=${username}`, {
                method: 'GET',
                headers: {
                    'accept': '*/*'
                }
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();
            return data.isAvailable;
        } catch (error) {
            console.error('Error checking username availability:', error);
            return false;
        }
    }

    const handleAnswer = async () => {
        setPasswordError('');
        setUsernameError('');
        setRepeatPasswordError('');

        if ('' === password) {
            setPasswordError('Please enter a password');
        }
        if ('' === repeatPassword) {
            setRepeatPasswordError('Please enter a password');
        }
        if ('' === username) {
            setUsernameError('Please enter a username');
        }
        if ('' !== password && '' !== repeatPassword && password !== repeatPassword) {
            setRepeatPasswordError('Passwords have to match');
        }
        if ('' === password || '' === repeatPassword || '' === username) {
            return;
        }

        const isUsernameAvailable = await checkUsernameAvailability(username);
        if (!isUsernameAvailable) {
            setUsernameError('Username is already taken');
            return;
        }

        if (checkLogin(password, repeatPassword)) {
            try {
                const response = await fetch('https://localhost:44399/api/User/Register', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'accept': '*/*'
                    },
                    body: JSON.stringify({name: username, password: password})
                });

                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }

                const data = await response.json();
                const token = data.token;
                localStorage.setItem('jwtToken', token);

                const decodedToken: any = jwtDecode(token);
                const usernameFromToken = decodedToken.name;

                navigate('/', {state: {username: usernameFromToken}});
            } catch (error) {
                console.error('There was a problem with the registration request:', error);
            }
        }
    }

    return (
        <div className="signup-page">
            <div className="content-wrapper">
                <div className="signup-container">
                    <div className="title-container">
                        <h2>Sign Up</h2>
                    </div>
                    <input
                        value={username}
                        placeholder="Enter your username here"
                        onChange={(usernameStr) => setUsername(usernameStr.target.value)}
                        className="input-box"
                    />
                    <label className="errorLabel">{usernameError}</label>
                    <input
                        value={password}
                        placeholder="Enter your password here"
                        type="password"
                        onChange={(passStr) => setPassword(passStr.target.value)}
                        className="input-box"
                    />
                    <label className="errorLabel">{passwordError}</label>
                    <input
                        value={repeatPassword}
                        placeholder="Repeat your password here"
                        type="password"
                        onChange={(passStr) => setRepeatPassword(passStr.target.value)}
                        className="input-box"
                    />
                    <label className="errorLabel">{repeatPasswordError}</label>
                    <button className="signup-button" onClick={() => handleAnswer()}>
                        Sign Up
                    </button>
                </div>
            </div>
            <div className="footer-text">
                <p>
                    Already have an account? <a href="/login">Log in</a>
                </p>
            </div>
        </div>
    );
};

export default SignupPage;