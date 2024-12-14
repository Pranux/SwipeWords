import React, { useEffect, useRef, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import './FlashcardDrop.css';

interface FlashcardProps {
  content: string;
  id: string;
  isCorrect: boolean;
  style: React.CSSProperties;
  position: number;
  onDragStart: (id: string) => void;
  onDragEnd: () => void;
}

const Flashcard: React.FC<FlashcardProps> = ({
                                               content,
                                               id,
                                               style,
                                               onDragStart,
                                               onDragEnd,
                                             }) => (
    <div
        className="flashcard-drop"
        draggable
        style={style}
        onDragStart={() => onDragStart(id)}
        onDragEnd={onDragEnd}
    >
      {content}
    </div>
);

function FlashcardDrop() {
  const [flashcards, setFlashcards] = useState<{
    isCorrect: boolean;
    id: string;
    content: string;
    position: number;
    style: React.CSSProperties;
  }[]>([]);
  const navigate = useNavigate();
  const location = useLocation();
  const [currentIndex, setCurrentIndex] = useState(0);
  const [error, setError] = useState<string | null>(null);
  const [elapsedTime, setElapsedTime] = useState(0);
  const [lives, setLives] = useState(5);
  const [draggingCardId, setDraggingCardId] = useState<string | null>(null);
  const difficulty = location.state.difficulty;
  const [updateSpeed, setUpdateSpeed] = useState(850);
  const [speed, setSpeed] = useState(3);

  useEffect(() => {
    if (difficulty === "Normal") {
      setLives(5);
      setUpdateSpeed(850);
      setSpeed(3);
    } else if (difficulty === "Easy") {
      setLives(10);
      setUpdateSpeed(1000);
      setSpeed(2);
    } else {
      setLives(1);
      setUpdateSpeed(700);
      setSpeed(4);
    }

    const timerInterval = setInterval(() => {
      setElapsedTime((prevTime) => prevTime + 1);
    }, 1000);

    return () => clearInterval(timerInterval);
  }, [difficulty]);


  useEffect(() => {
    const fetchFlashcard = async () => {
      try {
        const response = await fetch(
            `https://localhost:44399/api/flashcardDrop/GetFlashcardForDrop`
        );
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        const data = await response.json();

        const newFlashcard = {
          content: data.word,
          id: data.id,
          isCorrect: data.isCorrect,
          position: 0,
          style: {
            transform: `translateY(0vh)`,
            top: `0vh`,
          },
        };
        setFlashcards((prevFlashcards) => [...prevFlashcards, newFlashcard]);
      } catch (err) {
        setError((err as Error).message);
      }
    };

    const interval = setInterval(() => {
      if (currentIndex < 10) {
        fetchFlashcard();
        setCurrentIndex((prevIndex) => prevIndex + 1);
      } else {
        clearInterval(interval);
        setTimeout(() => {
          setCurrentIndex(0);
        }, 6000); 
      }
    }, updateSpeed);

    return () => clearInterval(interval);
  }, [currentIndex]);

  useEffect(() => {
    const updateInterval = setInterval(() => {
      setFlashcards((prevFlashcards) =>
          prevFlashcards.map((card) => ({
            ...card,
            position: card.position + 1
          }))
          
      );

      setFlashcards((prevFlashcards) =>
          prevFlashcards.map((card) => {
            if(difficulty === "Normal"){
              if (card.position === 36) {
                setLives((prevLives) => prevLives - 0.5);
                card.position += 100;
              }
            }
            else if(difficulty === "Easy"){
              if (card.position == 51) {
                setLives((prevLives) => prevLives - 0.5);
                card.position += 100;
              }
            }
            else if(difficulty === "Hard") {
              if (card.position == 25) {
                setLives((prevLives) => prevLives - 0.5);
              }
            }
            return card;
          })
      );
    }, 150);

    return () => clearInterval(updateInterval);
  }, []);

  const handleDragStart = (id: string) => {
    setDraggingCardId(id);
  };

  const handleDragEnd = () => {
    setDraggingCardId(null);
  };
  const handleDrop = (choice: boolean) => {
    if (draggingCardId !== null) {
      const droppedCard = flashcards.find((card) => card.id === draggingCardId);
      if (droppedCard && droppedCard.isCorrect !== choice) {
        setLives((prevLives) => prevLives - 1);
      }
      
      setFlashcards((prevFlashcards) =>
          prevFlashcards.map((card) =>
              card.id === draggingCardId
                  ? { ...card, position: card.position + 100 } 
                  : card
          )
      );
    }
  };

  if (error) {
    return <div>Error: {error}</div>;
  }

  if (lives === 0) {
    navigate('/flashcard-drop-results', { state: { elapsedTime, difficulty } });
  }

  return (
      <div className="flashcard-drop-container">
        <div
            className="divider left"
            onDragOver={(e) => e.preventDefault()}
            onDrop={() => handleDrop(false)}
        ></div>
        <div
            className="divider right"
            onDragOver={(e) => e.preventDefault()}
            onDrop={() => handleDrop(true)}
        ></div>
        <div className="timer">Time Elapsed: {elapsedTime}s</div>
        <div className="lives">Lives: {lives}</div>
        {flashcards.map((flashcard, index) => (
            <Flashcard
                key={flashcard.id}
                content={flashcard.content}
                id={flashcard.id}
                isCorrect={flashcard.isCorrect}
                onDragStart={handleDragStart}
                onDragEnd={handleDragEnd}
                position={flashcard.position}
                style={{
                  ...flashcard.style,
                  transform: `translateY(${flashcard.position * speed}vh)`,
                  transition: 'transform 0.3s ease-in-out'
                }}
            />
        ))}
        <div className="flashcard-indicators">
          <div className="indicator incorrect">
            <div className="arrow">←</div>
            INCORRECT
          </div>
          <div className="indicator correct">
            <div className="arrow">→</div>
            CORRECT
          </div>
        </div>
      </div>
  );
}

export default FlashcardDrop;