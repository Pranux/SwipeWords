import React from "react";
import {createBrowserRouter, Link} from "react-router-dom";

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
    return (
        <div className="app">
            <header className="app-header">
                <button onClick={getFlashcards}>Get flashcards</button>
                <p>{flashcards}</p>
                <hr style={{width: '100%', color: '#61dbfb'}}/>
                <div>
                    <Link to={'/flashcards-game'}>
                        Flashcards game
                    </Link>
                </div>
            </header>
        </div>
    )
}