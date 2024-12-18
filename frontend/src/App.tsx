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
import MemoryRecallHomePage from "./MemoryRecallHomePage";
import MemoryRecallReadPage from "./MemoryRecallReadPage";
import MemoryRecallPlacePage from "./MemoryRecallPlacePage";
import MemoryRecallResultPage from "./MemoryRecallResultPage";


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
    },
    {
        path: '/memory-recall-home',
        element: <MemoryRecallHomePage />
    },
    {
        path: '/memory-recall-read',
        element: <MemoryRecallReadPage />
    },
    {
        path: "/memory-recall-place",
        element: <MemoryRecallPlacePage />
    },
    {
        path: "/memory-recall-result",
        element: <MemoryRecallResultPage />
    }
]);

export default function App() {
    return <RouterProvider router={router} />;
}