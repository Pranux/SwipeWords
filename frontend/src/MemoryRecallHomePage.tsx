import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./MemoryRecallHomePage.css";
import Navbar from "./Navbar";

const MemoryRecallHomePage: React.FC = () => {
    const [wordCount, setWordCount] = useState<number>(0);
    const [placeholderPercentage, setPlaceholderPercentage] = useState<number>(0);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const fetchText = async () => {
        if (wordCount === 0 || placeholderPercentage === 0) {
            setError("Both values must be greater than 0.");
            return;
        }

        try {
            const queryString = new URLSearchParams({
                wordCount: wordCount.toString(),
                placeholderPercentage: placeholderPercentage.toString(),
            }).toString();

            const response = await fetch(
                `http://localhost:44398/api/memory-recall/fetch-and-process?${queryString}`,
                {
                    method: "POST",
                    headers: { "Accept": "*/*" },
                    body: null,
                }
            );

            if (response.ok) {
                const data = await response.json();
                if (data.originalText && data.textId) {
                    navigate("/memory-recall-read", {
                        state: {
                            textData: {
                                originalText: data.originalText,
                                textId: data.textId
                            },
                            wordCount,
                            placeholderPercentage,
                        },
                    });
                } else {
                    setError("No text or ID received from the server.");
                }
            } else {
                setError(`Failed to fetch text. Status code: ${response.status}`);
            }
        } catch (err) {
            setError("An error occurred while fetching text.");
        }
    };

    return (
        <div className="memory-recall-home">
            <Navbar />
            <div className="memory-recall-container">
                <h1>Welcome to Memory Recall</h1>
                <div className="input-section">
                    <div className="input-field">
                        <label>Word Count</label>
                        <input
                            type="number"
                            value={wordCount}
                            onChange={(e) => setWordCount(Math.max(0, Number(e.target.value)))}
                            placeholder="Enter word count"
                        />
                    </div>
                    <div className="input-field">
                        <label>Placeholder Percentage</label>
                        <input
                            type="number"
                            value={placeholderPercentage}
                            onChange={(e) =>
                                setPlaceholderPercentage(Math.min(75, Math.max(0, Number(e.target.value))))
                            }
                            placeholder="Enter percentage"
                        />
                    </div>
                </div>
                {error && <div className="error-message">{error}</div>}
                <button className="send-button" onClick={fetchText}>
                    Send Data
                </button>
            </div>
        </div>
    );
};

export default MemoryRecallHomePage;