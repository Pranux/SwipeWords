import React from 'react';
import { useNavigate } from 'react-router-dom';
import './Navbar.css';

function Navbar() {
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem('jwtToken'); // Remove token
        navigate('/login'); // Redirect to login page
    };

    return (
        <nav className="navbar">
            <div className="logo"><a href="/">Flashcard App</a></div>
            <ul>
                <li><a href="/">Home</a></li>
                <li><a href="/leaderboard">Leaderboard</a></li>
                <li><a onClick={handleLogout} href="/">Logout</a>
                </li>
            </ul>
        </nav>
    );
}

export default Navbar;
