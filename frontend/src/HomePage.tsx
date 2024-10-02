import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Navbar from './Navbar';
import FAQItem from './FAQItem';
import './App.css';
import './HomePage.css';

const HomePage = () => {
    const [counter, setCounter] = useState(0);
    const [expanded, setExpanded] = useState<number | null>(null);
    const navigate = useNavigate();

    const faqData = [
        {
            question: "What are the rules of the game?",
            answer: "In this game, you will be shown flashcards with words. Your goal is to guess the correct word based on the hints provided."
        },
        {
            question: "What’s the limit of the words?",
            answer: "The word limit in the game is between 0 and 200. You can choose any number of words within this range to begin the game."
        }
    ];

    const increment = (amount: number) => {
        setCounter((prevCounter) => Math.min(prevCounter + amount, 200));
    };

    const decrement = (amount: number) => {
        setCounter((prevCounter) => Math.max(prevCounter - amount, 0));
    };

    const startGame = () => {
        if (counter === 0) {
            alert("Please choose a word count greater than 0 to start the game.");
            return;
        }
        navigate(`/start?wordCount=${counter}`);
    };

    const toggleFAQ = (index: number) => {
        setExpanded(expanded === index ? null : index);
    };

    return (
        <div className="App">
            <Navbar />

            <div className="welcome-section">
                <h1>Welcome to the Flashcard Application</h1>
                <p>Choose the number of words to start the game:</p>

                <div className="counter-section">
                    <div className="counter-controls">
                        <button onClick={() => decrement(20)}>-20</button>
                        <button onClick={() => decrement(10)}>-10</button>
                        <button onClick={() => decrement(5)}>-5</button>
                        <button onClick={() => decrement(1)}>-</button>

                        <span className="counter-value">{counter}</span>

                        <button onClick={() => increment(1)}>+</button>
                        <button onClick={() => increment(5)}>+5</button>
                        <button onClick={() => increment(10)}>+10</button>
                        <button onClick={() => increment(20)}>+20</button>
                    </div>
                    <button className="start-button" onClick={startGame}>
                        Start Game
                    </button>
                </div>
            </div>

            <div className="faq-section">
                <h2>Frequently Asked Questions</h2>
                {faqData.map((item, index) => (
                    <FAQItem
                        key={index}
                        question={item.question}
                        answer={item.answer}
                        index={index}
                        expanded={expanded}
                        toggleFAQ={toggleFAQ}
                    />
                ))}
            </div>
        </div>
    );
};

export default HomePage;