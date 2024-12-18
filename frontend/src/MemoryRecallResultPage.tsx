import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import Navbar from "./Navbar";
import "./MemoryRecallResultPage.css";

interface CategorizedWord {
    word: string;
    isCorrect: boolean;
}

const MemoryRecallResultPage: React.FC = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { result, userInputs } = location.state || {};

    if (!result || !userInputs) {
        return (
            <div className="recall-results-container">
                <Navbar />
                <div className="recall-results-box">
                    <h1 className="recall-title">Recall Results</h1>
                    <p>No results available.</p>
                    <button className="recall-back-button" onClick={() => navigate("/")}>
                        Back to Home
                    </button>
                </div>
            </div>
        );
    }

    const { correctWords, score } = result;
    const correctSet = new Set<string>(correctWords);

    const categorizedWords: CategorizedWord[] = userInputs.map((word: string) => ({
        word,
        isCorrect: correctSet.has(word),
    }));

    return (
        <div className="recall-results-container">
            <Navbar />
            <div className="recall-results-box">
                <h1 className="recall-title">Recall Results</h1>
                <h2 className="recall-score">Score: {score}</h2>

                <div className="recall-result-wrapper">
                    {/* Correct Answers */}
                    <div className="recall-correct">
                        <h3>Correct Answers</h3>
                        <div className="word-box">
                            {categorizedWords
                                .filter((entry) => entry.isCorrect)
                                .map((entry, index) => (
                                    <div key={index} className="correct-box">
                                        {entry.word}
                                    </div>
                                ))}
                        </div>
                    </div>

                    <div className="recall-incorrect">
                        <h3>Incorrect Answers</h3>
                        <div className="word-box">
                            {categorizedWords
                                .filter((entry) => !entry.isCorrect)
                                .map((entry, index) => (
                                    <div key={index} className="incorrect-box">
                                        {entry.word}
                                    </div>
                                ))}
                        </div>
                    </div>
                </div>

                <button className="recall-back-button" onClick={() => navigate("/")}>
                    Back to Home
                </button>
            </div>
        </div>
    );
};

export default MemoryRecallResultPage;