import React from 'react';
import { useAuth } from '../context/AuthContext';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import { LogOut, BookOpen, BarChart } from 'lucide-react';
import SeedData from './SeedData';

const Layout: React.FC = () => {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <div style={{ display: 'flex', minHeight: '100vh', width: '100%' }}>
            <aside style={{ width: '250px', backgroundColor: '#1a1a1a', padding: '20px', display: 'flex', flexDirection: 'column' }}>
                <h2 style={{ marginBottom: '40px', color: '#646cff' }}>LearnIT Admin</h2>

                <nav style={{ flex: 1 }}>
                    <Link to="/dashboard" style={{ display: 'flex', alignItems: 'center', gap: '10px', color: '#ccc', textDecoration: 'none', padding: '10px', borderRadius: '6px', marginBottom: '5px' }}>
                        <BookOpen size={20} />
                        <span>Cursos</span>
                    </Link>
                    <Link to="/metrics" style={{ display: 'flex', alignItems: 'center', gap: '10px', color: '#ccc', textDecoration: 'none', padding: '10px', borderRadius: '6px' }}>
                        <BarChart size={20} />
                        <span>Métricas</span>
                    </Link>
                    {/* Future: Lessons link if needed separate */}
                </nav>

                <div style={{ borderTop: '1px solid #333', paddingTop: '20px' }}>
                    <div style={{ marginBottom: '10px', fontSize: '14px', color: '#888' }}>
                        {user?.fullName}
                    </div>
                    <button onClick={handleLogout} style={{ display: 'flex', alignItems: 'center', background: 'transparent', border: 'none', color: '#ff6b6b', cursor: 'pointer', padding: 0 }}>
                        <LogOut size={20} style={{ marginRight: '10px' }} />
                        Cerrar Sesión
                    </button>
                </div>
            </aside>

            <main style={{ flex: 1, padding: '40px', backgroundColor: '#242424', overflowY: 'auto' }}>
                <Outlet />
                <SeedData />
            </main>
        </div>
    );
};

export default Layout;
