import React from 'react';
import Navbar from './Navbar';

function GamePage() {
    return (
        <div className="App">
            <Navbar />
            <div className="content">
                <h1>Flashcard Game</h1>
            </div>
        </div>
    );
}

export default GamePage