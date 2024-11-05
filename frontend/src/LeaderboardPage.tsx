import React, { useEffect, useState } from 'react';
import Navbar from './Navbar';
import './Leaderboard.css';

interface LeaderboardEntry {
    userName: string;
    maxScore: number;
}

const Leaderboard = () => {
    const [leaderboardData, setLeaderboardData] = useState<LeaderboardEntry[]>([]);

    useEffect(() => {
        const fetchLeaderboard = async () => {
            try {
                const response = await fetch('https://localhost:44399/api/Leaderboard/GetLeaderboard?top=10', {
                    method: 'GET',
                    headers: {
                        'accept': '*/*'
                    }
                });

                if (!response.ok) {
                    throw new Error(`Network response was not ok: ${response.statusText}`);
                }

                const data: LeaderboardEntry[] = await response.json();
                setLeaderboardData(data);
            } catch (error) {
                console.error('Error fetching leaderboard data:', error);
            }
        };

        fetchLeaderboard();
    }, []);

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
                    </tr>
                    </thead>
                    <tbody>
                    {leaderboardData.map((entry, index) => (
                        <tr key={index}>
                            <td>{entry.userName}</td>
                            <td>{entry.maxScore}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default Leaderboard;