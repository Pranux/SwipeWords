import React, {useState} from "react";
import './LoginPage.css';
import {useNavigate} from "react-router-dom";

const SignupPage = () => {
    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [repeatPassword, setRepeatPassword] = useState<string>('');
    const [usernameError, setUsernameError] = useState<string>('');
    const [passwordError, setPasswordError] = useState<string>('');
    const [repeatPasswordError, setRepeatPasswordError] = useState<string>('');
    const [loggedIn, setLoggedIn] = useState<boolean>(false)
    const navigate = useNavigate();

    const checkLogin = (username : string, password : string, repeatPassword : string) => {
        return password === repeatPassword;
    }

    const handleAnswer = (isLoggedIn : boolean) => {

        setPasswordError('');
        setUsernameError('');
        if('' === password) {
            setPasswordError('Please enter a password');
        }
        if('' === repeatPassword) {
            setRepeatPasswordError('Please enter a password');
        }
        if('' === username) {
            setUsernameError('Please enter a username');
        }
        if('' !== password && '' !== repeatPassword && password !== repeatPassword) {
            setRepeatPasswordError('Passwords have to match');        }
        if('' === password || '' === repeatPassword || '' === username) {
            return;
        }

        if(checkLogin(username, password, repeatPassword) === true){
            setLoggedIn(isLoggedIn);
            navigate('/');
        }
    }

    return (
        <div className="signup-page">
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
                <button className="signup-button" onClick={() => handleAnswer(true)}>
                    Sign Up
                </button>
            </div>
        </div>);
};

export default SignupPage;