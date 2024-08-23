import React from 'react';

const Loading = ({ progress = 10 }) => {
    return (
        <div className="progress-modal-overlay">
            <div className="progress-modal-content">
                <h2>Loading...</h2>
                <div className="progress-bar">
                    <div
                        className="progress-bar-fill"
                        style={{ width: `${progress}%` }}
                    ></div>
                </div>
                <p>{progress}%</p>
            </div>
        </div>
    );
};
export default Loading;
