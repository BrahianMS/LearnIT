import React, { useEffect, useState } from 'react';
import api from '../services/api';
import { BarChart, Book, FileText, CheckCircle } from 'lucide-react';

interface Stats {
    totalCourses: number;
    publishedCourses: number;
    draftCourses: number;
    totalLessons: number;
}

const Metrics: React.FC = () => {
    const [stats, setStats] = useState<Stats | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchStats = async () => {
            try {
                const response = await api.get('/stats/dashboard');
                setStats(response.data);
            } catch (error) {
                console.error('Error fetching stats', error);
            } finally {
                setLoading(false);
            }
        };

        fetchStats();
    }, []);

    if (loading) return <div>Cargando métricas...</div>;
    if (!stats) return <div>No se pudieron cargar las métricas.</div>;

    return (
        <div>
            <h1 style={{ color: 'white', marginBottom: '30px' }}>Dashboard de Métricas</h1>

            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(240px, 1fr))', gap: '20px' }}>
                <div style={{ backgroundColor: '#2a2a2a', padding: '25px', borderRadius: '8px', border: '1px solid #444', display: 'flex', alignItems: 'center', gap: '20px' }}>
                    <div style={{ padding: '15px', borderRadius: '50%', backgroundColor: '#646cff20', color: '#646cff' }}>
                        <Book size={32} />
                    </div>
                    <div>
                        <div style={{ fontSize: '32px', fontWeight: 'bold', color: 'white' }}>{stats.totalCourses}</div>
                        <div style={{ color: '#888' }}>Total Cursos</div>
                    </div>
                </div>

                <div style={{ backgroundColor: '#2a2a2a', padding: '25px', borderRadius: '8px', border: '1px solid #444', display: 'flex', alignItems: 'center', gap: '20px' }}>
                    <div style={{ padding: '15px', borderRadius: '50%', backgroundColor: '#2ecc7120', color: '#2ecc71' }}>
                        <CheckCircle size={32} />
                    </div>
                    <div>
                        <div style={{ fontSize: '32px', fontWeight: 'bold', color: 'white' }}>{stats.publishedCourses}</div>
                        <div style={{ color: '#888' }}>Publicados</div>
                    </div>
                </div>

                <div style={{ backgroundColor: '#2a2a2a', padding: '25px', borderRadius: '8px', border: '1px solid #444', display: 'flex', alignItems: 'center', gap: '20px' }}>
                    <div style={{ padding: '15px', borderRadius: '50%', backgroundColor: '#f1c40f20', color: '#f1c40f' }}>
                        <FileText size={32} />
                    </div>
                    <div>
                        <div style={{ fontSize: '32px', fontWeight: 'bold', color: 'white' }}>{stats.draftCourses}</div>
                        <div style={{ color: '#888' }}>Borradores</div>
                    </div>
                </div>

                <div style={{ backgroundColor: '#2a2a2a', padding: '25px', borderRadius: '8px', border: '1px solid #444', display: 'flex', alignItems: 'center', gap: '20px' }}>
                    <div style={{ padding: '15px', borderRadius: '50%', backgroundColor: '#e74c3c20', color: '#e74c3c' }}>
                        <BarChart size={32} />
                    </div>
                    <div>
                        <div style={{ fontSize: '32px', fontWeight: 'bold', color: 'white' }}>{stats.totalLessons}</div>
                        <div style={{ color: '#888' }}>Lecciones Totales</div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Metrics;
