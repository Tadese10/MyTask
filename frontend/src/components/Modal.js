import React from 'react';


function Modal({ isOpen, editing, onClose, children }) {
    if (!isOpen) return null;

    const overlayStyle = {
        position: 'fixed',
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        backgroundColor: 'rgba(0, 0, 0, 0.7)',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        zIndex: 1000,
    };

    const contentStyle = {
        backgroundColor: '#ffffff', // White background
        padding: '20px',
        borderRadius: '8px',
        position: 'relative',
        width: '80%',
        maxWidth: '700px',
        boxShadow: '0 4px 8px rgba(0, 0, 0, 0.2)',
        zIndex: 1001,
    };

    return (
        <div style={overlayStyle} onClick={onClose}>
            <div style={contentStyle} onClick={(e) => e.stopPropagation()}>
                <button className="close-button" onClick={onClose}>
                    &times;
                </button>
                {children}
            </div>
        </div>
    );
}

export default Modal;