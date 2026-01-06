import React, { createContext, useState, useContext, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';

interface User {
    userId: string;
    email: string;
    fullName: string;
}

interface AuthContextType {
    user: User | null;
    token: string | null;
    login: (token: string, user: User) => void;
    logout: () => void;
    isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<User | null>(null);
    const [token, setToken] = useState<string | null>(localStorage.getItem('token'));
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(!!token);

    useEffect(() => {
        if (token) {
            try {
                const decoded: any = jwtDecode(token);
                // Validar expiración si es necesario
                if (decoded.exp * 1000 < Date.now()) {
                    logout();
                } else {
                    if (!user) {
                        // Si recargamos, intentamos recuperar data del token si la tiene, o mejor usar la info guardada en LS si decidimos guardarla, o hacer fetch me.
                        // Por simplicidad en este assessment, asumimos que el user viene del login.
                        // Pero si recargo pierdo el user object si no lo guardo.
                        // Decoding token to recover basic info:
                        setUser({
                            userId: decoded.uid || decoded.sub,
                            email: decoded.email,
                            fullName: decoded.name || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
                        });
                    }
                    setIsAuthenticated(true);
                }
            } catch (e) {
                logout();
            }
        }
    }, [token]);

    const login = (newToken: string, newUser: User) => {
        localStorage.setItem('token', newToken);
        setToken(newToken);
        setUser(newUser);
        setIsAuthenticated(true);
    };

    const logout = () => {
        localStorage.removeItem('token');
        setToken(null);
        setUser(null);
        setIsAuthenticated(false);
    };

    return (
        <AuthContext.Provider value={{ user, token, login, logout, isAuthenticated }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};
