import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Navbar from './Navbar';
import './ResultsPage.css';

interface Result {
    word: string;
    correct: boolean;
}

const ResultsPage = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const results = (location.state?.results || []) as Result[];

    const handlePlayAgain = () => {
        navigate('/');
    };

    return (
        <div>
            <Navbar />
            <h1>Game Results</h1>
            <p>You marked {results.filter(result => result.correct).length} out of {results.length} correct!</p>
            <ul>
                {results.map((result: Result, index: number) => (
                    <li key={index} style={{ color: result.correct ? 'green' : 'red' }}>
                        {result.word} - {result.correct ? 'Correct' : 'Incorrect'}
                    </li>
                ))}
            </ul>
            <button onClick={handlePlayAgain}>Play Again</button>
        </div>
    );
};

export default ResultsPage;