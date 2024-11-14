import React, { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useSwipeable } from 'react-swipeable';
import './GamePage.css';

const GamePage = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const [wordList, setWordList] = useState<string[]>([]);
    const [currentWordIndex, setCurrentWordIndex] = useState(0);
    const [results, setResults] = useState<{ word: string, correct: boolean }[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [flashcardId, setFlashcardId] = useState<string>(''); // State for flashcard ID
    const [dragDistance, setDragDistance] = useState(0); 
    const [isAnimating, setIsAnimating] = useState(false); 
    const maxDragDistance = 200;

    useEffect(() => {
        const query = new URLSearchParams(location.search);
        const wordCount = parseInt(query.get('wordCount') || '10', 10);

        const fetchFlashcards = async () => {
            try {
                const response = await fetch(`https://localhost:44399/api/Flashcards/GetFlashcards?wordCount=${wordCount}`);
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                setWordList(data.mixedWords); // Use 'mixedWords' from the response
                setFlashcardId(data.flashcardId); // Use 'flashcardId' from the response
                console.log('Flashcard ID in GamePage:', data.flashcardId); // Log the flashcard ID
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
        setIsAnimating(true); 

        setTimeout(() => {
            setIsAnimating(false); 
            setDragDistance(0); 
            if (currentWordIndex < wordList.length - 1) {
                setCurrentWordIndex(currentWordIndex + 1);
            } else {
                navigate('/results', { state: { results: [...results, { word: currentWord, correct: isCorrect }], flashcardId } });
            }
        }, 300); 
    };

    const handleEndNow = () => {
        navigate('/results', { state: { results, flashcardId } });
    };

    const swipeHandlers = useSwipeable({
        onSwiping: (eventData) => {
            const limitedDrag = Math.min(Math.max(eventData.deltaX, -maxDragDistance), maxDragDistance);
            setDragDistance(limitedDrag);
        },
        onSwipedLeft: () => {
            if (dragDistance < -maxDragDistance * 0.5) {
                handleAnswer(false);
            } 
        },
        onSwipedRight: () => {
            if (dragDistance > maxDragDistance * 0.5) {
                handleAnswer(true);
            } 
        },
        onSwiped: () => setDragDistance(0),        // Reset drag distance after swipe
        preventScrollOnSwipe: true,
        trackMouse: true
    });

    const opacity = Math.min(Math.abs(dragDistance) / maxDragDistance, 0.5);
    const backgroundColor = dragDistance > 0 
        ? `rgba(0, 255, 0, ${opacity})`  // Green overlay for correct swipe
        : `rgba(255, 0, 0, ${opacity})`; // Red overlay for incorrect swipe

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <div className="game-page">
            <div className="flashcard-placeholder" />
            <div
                className={`flashcard ${isAnimating ? 'animate-out' : ''}`}
                {...swipeHandlers}
                style={{
                    transform: `translateX(${dragDistance}px) rotate(${dragDistance * 0.05}deg)`,
                    backgroundColor: backgroundColor,
                    transition: isAnimating ? 'transform 0.3s ease' : 'none'
                }}
            >
                <h2 className="flashcard-text">{wordList[currentWordIndex]}</h2>
            </div>
            <button className="end-now-button" onClick={handleEndNow}>End Now</button>
        </div>
    );
};

export default GamePage;