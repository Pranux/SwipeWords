import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import './GamePage.css';

const GamePage = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const [wordList, setWordList] = useState<string[]>([]);
    const [currentWordIndex, setCurrentWordIndex] = useState(0);
    const [results, setResults] = useState<{ word: string, correct: boolean }[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const query = new URLSearchParams(location.search);
        const wordCount = parseInt(query.get('wordCount') || '10');

        const fetchFlashcards = async () => {
            try {
                const response = await fetch(`http://localhost:5212/api/Flashcards/GetFlashcards?wordCount=${wordCount}`);
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const jsonWords: string[] = await response.json();
                setWordList(jsonWords);
            } catch (err) {
                setError((err as Error).message);
            } finally {
                setLoading(false);
            }
        };

        fetchFlashcards();
    }, [location.search]);

    const handleAnswer = (isCorrect: boolean) => {
        const currentWord = wordList[currentWordIndex];
        setResults(prev => [...prev, { word: currentWord, correct: isCorrect }]);
        if (currentWordIndex < wordList.length - 1) {
            setCurrentWordIndex(currentWordIndex + 1);
        } else {
            navigate('/results', { state: { results: [...results, { word: currentWord, correct: isCorrect }] } });
        }
    };

    const handleEndNow = () => {
        navigate('/results', { state: { results } });
    };

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <div className="game-page">
            <div className="flashcard">
                <h2>{wordList[currentWordIndex]}</h2>
            </div>
            <div className="buttons">
                <button className="correct-button" onClick={() => handleAnswer(true)}>Correct</button>
                <button className="incorrect-button" onClick={() => handleAnswer(false)}>Incorrect</button>
            </div>
            <button className="end-now-button" onClick={handleEndNow}>End Now</button>
        </div>
    );
};

export default GamePage;