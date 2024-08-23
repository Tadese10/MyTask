import React, { useState } from 'react';

function TimeRangePicker() {
    const [startTime, setStartTime] = useState('');
    const [endTime, setEndTime] = useState('');

    const handleStartTimeChange = (event) => {
        setStartTime(event.target.value);
    };

    const handleEndTimeChange = (event) => {
        setEndTime(event.target.value);
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        console.log(`Selected time range: ${startTime} - ${endTime}`);
        // Add your logic here to handle the time range (e.g., validation, API call)
    };

    return (
        <div>
            <h2>Time Range Picker</h2>
            <form onSubmit={handleSubmit}>
                <label>
                    Start Time:
                    <input
                        type="time"
                        value={startTime}
                        onChange={handleStartTimeChange}
                    />
                </label>
                <br />
                <label>
                    End Time:
                    <input
                        type="time"
                        value={endTime}
                        onChange={handleEndTimeChange}
                    />
                </label>
                <br />
                <button type="submit">Submit</button>
            </form>
            {startTime && endTime && (
                <p>
                    Selected Time Range: {startTime} - {endTime}
                </p>
            )}
        </div>
    );
}

export default TimeRangePicker;
