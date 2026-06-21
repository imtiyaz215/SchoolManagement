import { Link, Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';

export default function AppLayout() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  return (
    <div style={{ display: 'grid', gridTemplateColumns: '220px 1fr', minHeight: '100vh' }}>
      <aside style={{ background: '#001529', color: 'white', padding: 16 }}>
        <h3 style={{ color: 'white' }}>School Mgmt</h3>
        <nav style={{ display: 'flex', flexDirection: 'column', gap: 8 }}>
          <Link to="/dashboard" style={navStyle}>Dashboard</Link>
          <Link to="/students" style={navStyle}>Students</Link>
          <Link to="/academic" style={navStyle}>Academic</Link>
          <Link to="/behaviour" style={navStyle}>Behaviour</Link>
          <Link to="/settings" style={navStyle}>Reference Data</Link>
        </nav>
      </aside>
      <main>
        <header style={{ display: 'flex', justifyContent: 'space-between', padding: 12, borderBottom: '1px solid #eee', alignItems: 'center' }}>
          <span></span>
          <div>
            {user?.fullName} <span style={{ color: '#888' }}>({user?.role})</span>
            <button onClick={() => { logout(); navigate('/login'); }} style={{ marginLeft: 12, padding: '4px 12px' }}>Logout</button>
          </div>
        </header>
        <Outlet />
      </main>
    </div>
  );
}

const navStyle: React.CSSProperties = { color: 'white', textDecoration: 'none', padding: '8px 12px', borderRadius: 4 };
