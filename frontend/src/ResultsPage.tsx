import React, { useEffect, useState } from 'react';
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
    const flashcardId = location.state?.flashcardId || '';
    const [score, setScore] = useState<number | null>(null);
    const [correctWords, setCorrectWords] = useState<string[]>([]);
    const [incorrectWords, setIncorrectWords] = useState<string[]>([]);

    useEffect(() => {
        console.log('Results:', results);
        console.log('Flashcard ID:', flashcardId);

        const calculateScore = async () => {
            try {
                const userCorrect = results.filter(result => result.correct).map(result => result.word).join(',');
                const userIncorrect = results.filter(result => !result.correct).map(result => result.word).join(',');

                console.log('Request Payload:', {
                    id: flashcardId,
                    userCorrect,
                    userIncorrect,
                });

                const response = await fetch('https://localhost:44398/api/Flashcards/CalculateScore', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        flashcardId,
                        userCorrect,
                        userIncorrect,
                    }),
                });

                if (!response.ok) {
                    throw new Error(`Network response was not ok: ${response.statusText}`);
                }

                const data = await response.json();
                console.log('Score Data:', data);
                setScore(data.score);
                setCorrectWords(data.correctWords);
                setIncorrectWords(data.incorrectWords);
            } catch (error) {
                console.error('Error calculating score:', error);
            }
        };

        calculateScore();
    }, [results, flashcardId]);

    const handlePlayAgain = () => {
        navigate('/');
    };

    return (
        <div>
            <Navbar />
            <h1>Game Results</h1>
            {score !== null && (
                <div className="score-container">
                    <p className="score">Your score: {score}</p>
                    <div className="results-container">
                        <div className="correct-words">
                            <h2>Correct Words</h2>
                            {correctWords.map((word, index) => (
                                <p key={index} className="correct-word">{word}</p>
                            ))}
                        </div>
                        <div className="incorrect-words">
                            <h2>Incorrect Words</h2>
                            {incorrectWords.map((word, index) => (
                                <p key={index} className="incorrect-word">{word}</p>
                            ))}
                        </div>
                    </div>
                </div>
            )}
            <button onClick={handlePlayAgain}>Play Again</button>
        </div>
    );
};

export default ResultsPage;