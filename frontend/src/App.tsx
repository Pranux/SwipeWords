import React, { useState } from 'react';
import {
  createBrowserRouter,
  Link,
  RouterProvider
} from 'react-router-dom';
import './App.css';

const router = createBrowserRouter([
  {
    path: '/',
    element: <HomePage />,
  }
]);

function HomePage() {
  const [flashcards, setFlashcards] = React.useState('');
  const getFlashcards = async () => {
    try {
      const route = await fetch("https://localhost:44398/GetFlashcards");
      const json = await route.json();
      console.log(json);
      setFlashcards(json.summary);
    } catch (error) {
      console.log(error);
    }
  }

    // State to keep track of the current flashcard index
    const [currentCard, setCurrentCard] = useState(0);

    // Array of flashcards (can be numbers or any content)
    const tempFlashcards = [1, 2, 3, 4, 5];

    // Event handler to go to the next flashcard
    const handleNext = () => {
        if (currentCard < tempFlashcards.length - 1) {
            setCurrentCard(currentCard + 1);
        }
    };
  
  return (

      <div className="App">
          <nav className="navbar">
              <div className="logo">Flashcard App</div>
              <ul>
                  <li><a href="#home">Home</a></li>
                  <li><a href="#about">About</a></li>
                  <li><a href="#contact">Contact</a></li>
              </ul>
          </nav>
          <div className="flashcard">
              <h1>{tempFlashcards[currentCard]}</h1>
          </div>
          <div className="buttons">
              <button onClick={handleNext} disabled={currentCard === flashcards.length - 1}>
              Not OK
              </button>
              <button onClick={handleNext} disabled={currentCard === flashcards.length - 1}>
                  OK
              </button>
          </div>
      </div>
  );
}

export default function App() {
    return <RouterProvider router={router}/>;
}