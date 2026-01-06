import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';

const Login: React.FC = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');

        try {
            const response = await api.post('/auth/login', { email, password });
            const { token, userId, email: userEmail, fullName } = response.data;

            login(token, { userId, email: userEmail, fullName });
            navigate('/dashboard');
        } catch (err: any) {
            setError('Credenciales inválidas o error de servidor');
            console.error(err);
        }
    };

    return (
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh', width: '100%', backgroundColor: '#1a1a1a' }}>
            <div style={{ backgroundColor: '#2a2a2a', padding: '40px', borderRadius: '8px', width: '400px', boxShadow: '0 4px 6px rgba(0,0,0,0.3)' }}>
                <h1 style={{ textAlign: 'center', marginBottom: '30px', color: '#646cff' }}>LearnIT</h1>

                {error && <div style={{ backgroundColor: '#ff6b6b20', color: '#ff6b6b', padding: '10px', borderRadius: '4px', marginBottom: '20px', textAlign: 'center' }}>{error}</div>}

                <form onSubmit={handleSubmit}>
                    <div style={{ marginBottom: '20px' }}>
                        <label style={{ display: 'block', marginBottom: '8px', color: '#ccc' }}>Email</label>
                        <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            style={{ width: '100%', padding: '10px', borderRadius: '4px', border: '1px solid #444', backgroundColor: '#333', color: 'white', boxSizing: 'border-box' }}
                            required
                        />
                    </div>

                    <div style={{ marginBottom: '30px' }}>
                        <label style={{ display: 'block', marginBottom: '8px', color: '#ccc' }}>Password</label>
                        <input
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            style={{ width: '100%', padding: '10px', borderRadius: '4px', border: '1px solid #444', backgroundColor: '#333', color: 'white', boxSizing: 'border-box' }}
                            required
                        />
                    </div>

                    <button type="submit" style={{ width: '100%', padding: '12px', borderRadius: '4px', border: 'none', backgroundColor: '#646cff', color: 'white', fontWeight: 'bold', cursor: 'pointer' }}>
                        Ingresar
                    </button>
                </form>
            </div>
        </div>
    );
};

export default Login;
