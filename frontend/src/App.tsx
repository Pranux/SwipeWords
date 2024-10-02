import React from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import HomePage from './HomePage';
import GamePage from './GamePage';
import LeaderboardPage from './LeaderboardPage';
import './App.css';
import  './ResultsPage';
import ResultsPage from "./ResultsPage";

const router = createBrowserRouter([
    {
        path: '/',
        element: <HomePage />,
    },
    {
        path: '/start',
        element: <GamePage />,
    },
    {
        path: '/leaderboard',
        element: <LeaderboardPage />,
    },
    {
        path: '/results',
        element: <ResultsPage />,
    }
]);

export default function App() {
    return <RouterProvider router={router} />;
}