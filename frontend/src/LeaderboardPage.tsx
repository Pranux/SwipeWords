import React from 'react';
import Navbar from './Navbar';
import './Leaderboard.css';

const Leaderboard = () => {
    // Sample data for the leaderboard
    const leaderboardData = [
        { username: 'John', score: 150, date: '2024-10-01' },
        { username: 'Jane', score: 130, date: '2024-09-30' },
        { username: 'PlayerOne', score: 110, date: '2024-09-29' },
        { username: 'UserTwo', score: 100, date: '2024-09-28' },
    ];

    return (
        <div className="leaderboard-page">
            <Navbar />

            <div className="leaderboard-container">
                <h1>Leaderboard</h1>
                <table className="leaderboard-table">
                    <thead>
                    <tr>
                        <th>Username</th>
                        <th>Score</th>
                        <th>Date</th>
                    </tr>
                    </thead>
                    <tbody>
                    {leaderboardData.map((entry, index) => (
                        <tr key={index}>
                            <td>{entry.username}</td>
                            <td>{entry.score}</td>
                            <td>{entry.date}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default Leaderboard;