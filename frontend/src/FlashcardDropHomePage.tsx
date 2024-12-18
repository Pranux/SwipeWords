import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import './HomePage.css';
import './FlashcardDropHomePage.css'
import Navbar from './Navbar';

const FlashcardDropHomePage = () => {
    const navigate = useNavigate();
    const [difficulty, setDifficulty] = useState("Normal");
    const startGame = () => {
        navigate('/flashcard-drop-start', { state: { difficulty } });
    };

    return (
        <div>
            <Navbar />

            <div className="welcome-section">
                <h1>Welcome to Flashcard Drop</h1>
                <p>Choose the difficulty:</p>
                <div className="counter-section">
                    <div className="choice-controls">
                        <button onClick={() => setDifficulty("Easy")}>Easy</button>
                        <button onClick={() => setDifficulty("Normal")}>Normal</button>
                        <button onClick={() => setDifficulty("Hard")}>Hard</button>
                    </div>
                    <span className="counter-value">{difficulty}</span>
                    <button className="start-button" onClick={startGame}>
                        Start Game
                    </button>
                </div>
            </div>
        </div>
    );
};

export default FlashcardDropHomePage;