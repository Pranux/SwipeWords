import React, { useState } from "react";
import './LoginPage.css';
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";


const LoginPage = () => {
    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [usernameError, setUsernameError] = useState<string>('');
    const [passwordError, setPasswordError] = useState<string>('');
    const navigate = useNavigate();

    const handleAnswer = async () => {
        setPasswordError('');
        setUsernameError('');

        if ('' === password) {
            setPasswordError('Please enter a password');
        }
        if ('' === username) {
            setUsernameError('Please enter a username');
        }
        if ('' === password || '' === username) {
            return;
        }

        try {
            const response = await fetch('https://localhost:44399/api/User/Login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'accept': '*/*'
                },
                body: JSON.stringify({ name: username, password: password })
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();
            const token = data.token;
            localStorage.setItem('jwtToken', token);

            const decodedToken: any = jwtDecode(token);
            const usernameFromToken = decodedToken.name;
            
            navigate('/', { state: { username: usernameFromToken } });
        } catch (error) {
            console.error('There was a problem with the login request:', error);
        }
    };

    return (
        <div className="login-page">
            <div className="content-wrapper">
                <div className="login-container">
                    <div className="title-container">
                        <h2>Login</h2>
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
                    <button className="login-button" onClick={() => handleAnswer()}>
                        Log in
                    </button>
                </div>
                <div className="login-container info-container">
                    <h2>About Swipe Words</h2>
                    <p>
                        Swipe Words is a web application / game, where you can test your speed, reaction time and even
                        English vocabulary. Firstly you will need to log in and then you will be transported to the main
                        page.
                        There you will have the option to select how many words you want and play the game. try to get
                        as many
                        points as you can and see yourself in a leaderboard!
                        <br/>
                        <br/>
                        If you don't have and account click <u><i>Sign up</i></u> button bellow.
                    </p>
                </div>
            </div>
            <div className="footer-text">
                <p>
                    Don't have an account? <a href="/signup">Sign Up</a>
                </p>
            </div>
        </div>
    );
};

export default LoginPage;