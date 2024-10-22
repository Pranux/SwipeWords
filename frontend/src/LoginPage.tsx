import React, {useState} from "react";
import './LoginPage.css';
import {useNavigate} from "react-router-dom";

const LoginPage = () => {
    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [loggedIn, setLoggedIn] = useState<boolean>(false)
    const navigate = useNavigate();
    
    const checkLogin = (username : string, password : string) => {
        return true;
    }
    
    const handleAnswer = (isLoggedIn : boolean) => {
        if(checkLogin(username, password) === true){
            setLoggedIn(isLoggedIn);
            navigate('/');
        }
    }
    
    return (
        <div className="login-page">
            <div className="title-container">
                <h2>Login</h2>
            </div>
            <br/>
            <input
                value={username}
                placeholder="Enter your username here"
                onChange={(usernameStr) => setUsername(usernameStr.target.value)}
                className="input-box"
            />
            <br/>
            <input
                value={password}
                placeholder="Enter your password here"
                onChange={(passStr) => setPassword(passStr.target.value)}
                className="input-box"
            />
            <br/>
            <button
                className="login-button"
                onClick={() => handleAnswer(true)}>Log in
            </button>
        </div>
    );
};

export default LoginPage;