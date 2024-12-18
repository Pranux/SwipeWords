import React, { useEffect, useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import Navbar from "./Navbar";
import "./MemoryRecallReadPage.css";

const MemoryRecallReadPage: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const { textData, wordCount, placeholderPercentage } = location.state || {};

    const [timeLeft, setTimeLeft] = useState<number>(calculateReadTime(wordCount || 0, placeholderPercentage || 0));

    function calculateReadTime(words: number, percentage: number): number {
        const baseTime = 10;
        const factor = 1.5;
        return Math.ceil(baseTime + words * (1 + percentage / 100) * factor);
    }

    useEffect(() => {
        const timer = setInterval(() => {
            setTimeLeft((prev) => {
                if (prev <= 1) {
                    clearInterval(timer);
                    navigateToPlaceholdersPage();
                }
                return prev - 1;
            });
        }, 1000);

        return () => clearInterval(timer);
    }, []);

    const navigateToPlaceholdersPage = () => {
        navigate("/memory-recall-place", {
            state: {
                textId: textData?.textId,
                wordCount,
                placeholderPercentage,
            },
        });
    };

    return (
        <div className="memory-recall-read">
            <Navbar />
            <div className="content-container">
                <h1>Read and Remember</h1>
                <div className="timer">Time Left: {timeLeft} seconds</div>
                <div className="text-display">{textData?.originalText || "No text available."}</div>
                <button className="skip-button" onClick={navigateToPlaceholdersPage}>
                    Skip Reading
                </button>
            </div>
        </div>
    );
};

export default MemoryRecallReadPage;