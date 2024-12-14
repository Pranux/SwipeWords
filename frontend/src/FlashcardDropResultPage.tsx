import React, { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import './ResultsPage.css';
import Navbar from './Navbar';

const FlashcardDropResultPage = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const elapsedTime = location.state?.elapsedTime;
    const difficulty = location.state?.difficulty;
    const [score, setScore] = useState(0);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const calculateScore = async () => {
            try {
                const response = await fetch('https://localhost:44399/api/FlashcardDrop/CalculateScore', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        elapsedTime,
                        difficulty,
                    }),
                });

                if (!response.ok) {
                    throw new Error(`Network response was not ok: ${response.statusText}`);
                }
                
                const data = await response.json();
                setScore(data.score);
                
                
            } catch (err) {
                setError((err as Error).message);
            }
        };
        calculateScore();
    });
    const handlePlayAgain = () => {
        navigate('/flashcard-drop-home');
    };

    if (error) {
        return <div>Error: {error}</div>;
    }
    
    return (
        <div>
            <Navbar/>
            <h1>Game Results</h1>
                <div className="score-container">
                    <p className="score">Your score: {score}</p>
                </div>
            <button onClick={handlePlayAgain}>Play Again</button>
        </div>
    );
}

export default FlashcardDropResultPage;