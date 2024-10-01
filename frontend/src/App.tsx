import React, { useState } from 'react';
import {
  createBrowserRouter,
  RouterProvider
} from 'react-router-dom';
import './App.css';

const router = createBrowserRouter([
    {
        path: '/',
        element: <HomePage />,
    }, 
    {
        path: '/flash-card-game-1',
        element: <HomePage />,
    },
    {
        path: '/flash-card-game-2',
        element: <HomePage />,
    },
    {
        path: '/flash-card-game-3',
        element: <HomePage />,
    },
]);

function HomePage() {
  const [flashcards, setFlashcards] = React.useState('');
  const getFlashcards = async () => {
    try {
      const route = await fetch("https://localhost:44398/api/Flashcards/GetFlashcards?wordCount=5");
      const json = await route.json();
      console.log(json);
      setFlashcards(json);
    } catch (error) {
      console.log(error);
    }
  }

    // State to keep track of the current flashcard index
    const [currentCard, setCurrentCard] = useState(0);

    // Array of flashcards (can be numbers or any content)
    var tempFlashcards = [1, 2, 3, 4, 5];

    // Event handler to go to the next flashcard
    const handleNext = () => {
        setCurrentCard((prevCard) => (prevCard + 1) % tempFlashcards.length);
    };
  
  return (

      <div className="App">
          <nav className="navbar">
              <div className="logo"><a href="/">Flashcard App</a></div>
              <ul>
                  <li><a href="flash-card-game-1">Flash card game #1</a></li>
                  <li><a href="flash-card-game-2">Flash card game #2</a></li>
                  <li><a href="flash-card-game-3">Flash card game #3</a></li>
              </ul>
          </nav>
          <div className="flashcard">
              <h1>{tempFlashcards.length > 0 ? tempFlashcards[currentCard] : 'No flashcards available'}</h1>
          </div>
          <div className="buttons">
              <button onClick={handleNext}>
              Not OK
              </button>
              <button onClick={handleNext}>
                  OK
              </button>
          </div>
      </div>
  );
}

export default function App() {
    return <RouterProvider router={router}/>;
}