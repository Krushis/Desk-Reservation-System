import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import DeskPage from './pages/DeskPage';
import ProfilePage from './pages/ProfilePage';
import './App.css';

// prepared test user guid
const TEST_USER_ID = '11111111-1111-1111-1111-111111111111';

function App() {
  return (
    <Router>
      <div className="App">
        <nav className="navbar">
          <div className="nav-container">
            <h1>Desk Booking System</h1>
            <div className="nav-links">
              <Link to="/">Desks</Link>
              <Link to="/profile">My Profile</Link>
            </div>
          </div>
        </nav>

        <Routes>
          <Route path="/" element={<DeskPage userId={TEST_USER_ID} />} />
          <Route path="/profile" element={<ProfilePage userId={TEST_USER_ID} />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;