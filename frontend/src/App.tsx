import React from 'react';
import { createBrowserRouter, RouterProvider, Navigate } from 'react-router-dom';
import HomePage from './HomePage';
import GamePage from './GamePage';
import LeaderboardPage from './LeaderboardPage';
import ResultsPage from './ResultsPage';
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
import { jwtDecode } from "jwt-decode";
import './App.css';

const AuthGuard: React.FC<{ children: JSX.Element }> = ({ children }) => {
    const token = localStorage.getItem('jwtToken');

    if (!token) {
        console.warn('No token found. Redirecting to login.');
        return <Navigate to="/login" />;
    }

    try {
        const decoded: any = jwtDecode(token);
        const isTokenExpired = decoded.exp * 1000 < Date.now();
        if (isTokenExpired) {
            console.warn('Token expired. Redirecting to login.');
            localStorage.removeItem('jwtToken'); // Clear invalid token
            return <Navigate to="/login" />;
        }
    } catch (error) {
        console.error('Invalid token. Redirecting to login:', error);
        localStorage.removeItem('jwtToken'); // Clear invalid token
        return <Navigate to="/login" />;
    }
    
    return children;
};

// Router Configuration
const router = createBrowserRouter([
    {
        path: '/',
        element: (
            <AuthGuard>
                <HomePage />
            </AuthGuard>
        ),
    },
    {
        path: '/start',
        element: (
            <AuthGuard>
                <GamePage />
            </AuthGuard>
        ),
    },
    {
        path: '/leaderboard',
        element: (
            <AuthGuard>
                <LeaderboardPage />
            </AuthGuard>
        ),
    },
    {
        path: '/results',
        element: (
            <AuthGuard>
                <ResultsPage />
            </AuthGuard>
        ),
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
