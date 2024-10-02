import React from 'react';

interface FAQItemProps {
    question: string;
    answer: string;
    index: number;
    expanded: number | null;
    toggleFAQ: (index: number) => void;
}

const FAQItem: React.FC<FAQItemProps> = ({ question, answer, index, expanded, toggleFAQ }) => {
    return (
        <div className={`faq-item ${expanded === index ? 'expanded' : ''}`} onClick={() => toggleFAQ(index)}>
            <div className="faq-question">
                <h3>{question}</h3>
                <span className={`plus-icon ${expanded === index ? 'rotated' : ''}`}>+</span>
            </div>
            <div className="faq-answer">
                <p>{answer}</p>
            </div>
        </div>
    );
};

export default FAQItem;