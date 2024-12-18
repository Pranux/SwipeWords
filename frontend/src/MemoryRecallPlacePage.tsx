import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import Navbar from "./Navbar";
import "./MemoryRecallPlacePage.css";

const MemoryRecallPlacePage: React.FC = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { textId } = location.state || {};
    const [placeholderText, setPlaceholderText] = useState<string | null>(null);
    const [userInputs, setUserInputs] = useState<string[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

    useEffect(() => {
        if (!textId) {
            setError("Missing text ID.");
            return;
        }

        const fetchPlaceholderText = async () => {
            try {
                const response = await fetch(
                    `http://localhost:44398/api/memory-recall/get-placeholder-text/${textId}`
                );
                if (response.ok) {
                    const data = await response.json();
                    setPlaceholderText(data.placeholderText);
                    const placeholders = (data.placeholderText.match(/_/g) || []).map(() => "");
                    setUserInputs(placeholders);
                } else {
                    setError("Failed to fetch placeholder text.");
                }
            } catch (err) {
                setError("An error occurred while fetching placeholder text.");
            }
        };

        fetchPlaceholderText();
    }, [textId]);

    const handleInputChange = (index: number, value: string) => {
        const updatedInputs = [...userInputs];
        updatedInputs[index] = value;
        setUserInputs(updatedInputs);
    };

    const handleSubmit = async () => {
        setIsSubmitting(true);
        try {
            const response = await fetch(
                `http://localhost:44398/api/memory-recall/submit-recall?recallId=${textId}`,
                {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(userInputs),
                }
            );

            if (response.ok) {
                const result = await response.json();
                navigate("/memory-recall-result", { state: { result, userInputs } });
            } else {
                setError("Failed to submit recall inputs.");
            }
        } catch (err) {
            setError("An error occurred during submission.");
        } finally {
            setIsSubmitting(false);
        }
    };

    const renderTextWithInputs = () => {
        if (!placeholderText) return null;

        const parts = placeholderText.split(/_/g);
        return (
            <div
                style={{
                    fontSize: "1.2rem",
                    lineHeight: "1.5",
                    textAlign: "justify",
                    color: "#34495e",
                    marginBottom: "20px",
                }}
            >
                {parts.map((part, i) => (
                    <React.Fragment key={`fragment-${i}`}>
                        <span>{part}</span>
                        {i < userInputs.length && (
                            <input
                                type="text"
                                value={userInputs[i]}
                                onChange={(e) => handleInputChange(i, e.target.value)}
                                style={{
                                    width: "80px",
                                    height: "30px",
                                    margin: "0 5px",
                                    border: "2px solid #8e44ad",
                                    borderRadius: "5px",
                                    textAlign: "center",
                                    fontSize: "1rem",
                                    outline: "none",
                                    verticalAlign: "middle",
                                }}
                                maxLength={20}
                            />
                        )}
                    </React.Fragment>
                ))}
            </div>
        );
    };

    return (
        <div className="memory-recall-place">
            <Navbar />
            <div className="memory-recall-container">
                <h1 style={{ textAlign: "center", marginBottom: "20px" }}>
                    Fill in the Placeholders
                </h1>
                {error && (
                    <div
                        style={{
                            color: "red",
                            fontSize: "1rem",
                            marginBottom: "20px",
                            textAlign: "left",
                        }}
                    >
                        {error}
                    </div>
                )}
                {!error && placeholderText && renderTextWithInputs()}
                <button
                    className="save-button"
                    onClick={handleSubmit}
                    disabled={isSubmitting}
                >
                    {isSubmitting ? "Submitting..." : "Submit Recall"}
                </button>
            </div>
        </div>
    );
};

export default MemoryRecallPlacePage;