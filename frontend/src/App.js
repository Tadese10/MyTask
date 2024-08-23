import React from 'react'
import { BrowserRouter as Router, Routes, Route } from "react-router-dom"
import Dashboard from "./pages/Dashboard"
import Login from "./pages/Login"
import Registration from "./pages/Registration"

function App() {
  return (
    <Router>
      <Routes>
        <Route exact path="/" element={<Login />} />
        <Route exact path="/signup" element={<Registration />} />
        <Route exact path="/dashboard" element={<Dashboard />} />
      </Routes>
    </Router>
  );
}

export default App;