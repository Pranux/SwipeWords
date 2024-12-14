import React from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import HomePage from './HomePage';
import GamePage from './GamePage';
import LeaderboardPage from './LeaderboardPage';
import ResultsPage from "./ResultsPage";
import LoginPage from './LoginPage';
import SignupPage from './SignupPage';
import FlashcardDrop from './FlashcardDrop';
import FlashcardDropResults from './FlashcardDropResultPage';
import FlashcardDropHome from './FlashcardDropHomePage';
import './App.css';
import  './ResultsPage';


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
    },
    {
        path: '/login',
        element: <LoginPage />,
    },
    {
        path: '/signup',
        element: <SignupPage />,
    },
    {
        path: 'flashcard-drop-start',
        element: <FlashcardDrop />
    },
    {
        path: 'flashcard-drop-home',
        element: <FlashcardDropHome />
    },
    {
        path: 'flashcard-drop-results',
        element: <FlashcardDropResults />
    }
]);

export default function App() {
    return <RouterProvider router={router} />;
}