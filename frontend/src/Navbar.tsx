import React from 'react';
import './Navbar.css'

function Navbar() {
    return (
        <nav className="navbar">
            <div className="logo"><a href="/">Flashcard App</a></div>
            <ul>
                <li><a href="/flashcard-drop-home">Flashcard Drop</a></li>
                <li><a href="/">Home</a></li>
                <li><a href="/leaderboard">Leaderboard</a></li>
            </ul>
        </nav>
    );
}

export default Navbar;