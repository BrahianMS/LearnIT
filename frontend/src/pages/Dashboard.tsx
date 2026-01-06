import React, { useEffect, useState } from 'react';
import api from '../services/api';
import { Link } from 'react-router-dom';
import { Plus, Edit, Trash2 } from 'lucide-react';
import Modal from '../components/Modal';

interface Course {
    id: string;
    title: string;
    status: number; // 0: Draft, 1: Published
    lessonsCount: number;
}

const Dashboard: React.FC = () => {
    const [courses, setCourses] = useState<Course[]>([]);
    const [loading, setLoading] = useState(true);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [newCourseTitle, setNewCourseTitle] = useState('');
    const [creating, setCreating] = useState(false);

    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const pageSize = 9; // Grid 3x3 nicely

    const fetchCourses = async () => {
        setLoading(true);
        try {
            const response = await api.get('/courses', {
                params: { page, pageSize }
            });
            setCourses(response.data.items || []);
            setTotalPages(response.data.totalPages || 1);
        } catch (error) {
            console.error('Error fetching courses', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchCourses();
    }, [page]); // Reload when page changes

    const handleDelete = async (id: string) => {
        if (!confirm('¿Estás seguro de eliminar este curso?')) return;
        try {
            await api.delete(`/courses/${id}`);
            fetchCourses();
        } catch (error) {
            alert('Error al eliminar curso');
        }
    };

    const handleCreateSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!newCourseTitle.trim()) return;

        setCreating(true);
        try {
            await api.post('/courses', { title: newCourseTitle });
            setNewCourseTitle('');
            setIsModalOpen(false);
            fetchCourses();
        } catch (error) {
            alert('Error al crear curso');
        } finally {
            setCreating(false);
        }
    }

    const handlePageChange = (newPage: number) => {
        if (newPage > 0 && newPage <= totalPages) {
            setPage(newPage);
        }
    };

    if (loading && page === 1) return <div>Cargando cursos...</div>;

    return (
        <div>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
                <h1 style={{ color: 'white' }}>Mis Cursos</h1>
                <button
                    onClick={() => setIsModalOpen(true)}
                    style={{ display: 'flex', alignItems: 'center', gap: '5px', padding: '10px 15px', backgroundColor: '#646cff', border: 'none', borderRadius: '4px', color: 'white', cursor: 'pointer' }}
                >
                    <Plus size={18} /> Nuevo Curso
                </button>
            </div>

            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: '20px', minHeight: '400px' }}>
                {courses.length === 0 ? (
                    <p style={{ color: '#888' }}>No hay cursos en esta página.</p>
                ) : (
                    courses.map(course => (
                        <div key={course.id} style={{ backgroundColor: '#2a2a2a', padding: '20px', borderRadius: '8px', border: '1px solid #333', display: 'flex', flexDirection: 'column', justifyContent: 'space-between' }}>
                            <div>
                                <h3 style={{ marginTop: 0, color: '#eee' }}>{course.title}</h3>
                                <div style={{ marginBottom: '10px' }}>
                                    <span style={{ fontSize: '12px', padding: '4px 8px', borderRadius: '4px', backgroundColor: course.status === 1 ? '#2ecc7120' : '#f1c40f20', color: course.status === 1 ? '#2ecc71' : '#f1c40f' }}>
                                        {course.status === 1 ? 'Publicado' : 'Borrador'}
                                    </span>
                                </div>
                            </div>
                            <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '10px', marginTop: '10px' }}>
                                <Link to={`/courses/${course.id}`} style={{ color: '#646cff' }}><Edit size={18} /></Link>
                                <button onClick={() => handleDelete(course.id)} style={{ background: 'none', border: 'none', color: '#ff6b6b', cursor: 'pointer' }}><Trash2 size={18} /></button>
                            </div>
                        </div>
                    ))
                )}
            </div>

            {/* Pagination Controls */}
            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', marginTop: '30px', gap: '20px' }}>
                <button
                    onClick={() => handlePageChange(page - 1)}
                    disabled={page === 1}
                    style={{ padding: '8px 16px', background: '#333', border: '1px solid #555', color: 'white', borderRadius: '4px', cursor: page === 1 ? 'not-allowed' : 'pointer', opacity: page === 1 ? 0.5 : 1 }}
                >
                    Anterior
                </button>
                <span style={{ color: '#ccc' }}>Página {page} de {totalPages}</span>
                <button
                    onClick={() => handlePageChange(page + 1)}
                    disabled={page === totalPages}
                    style={{ padding: '8px 16px', background: '#333', border: '1px solid #555', color: 'white', borderRadius: '4px', cursor: page === totalPages ? 'not-allowed' : 'pointer', opacity: page === totalPages ? 0.5 : 1 }}
                >
                    Siguiente
                </button>
            </div>

            <Modal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                title="Crear Nuevo Curso"
            >
                <form onSubmit={handleCreateSubmit}>
                    <div style={{ marginBottom: '20px' }}>
                        <label style={{ display: 'block', marginBottom: '8px', color: '#ccc' }}>Título del Curso</label>
                        <input
                            type="text"
                            value={newCourseTitle}
                            onChange={(e) => setNewCourseTitle(e.target.value)}
                            placeholder="Ej. Introducción a TypeScript"
                            style={{ width: '100%', padding: '10px', borderRadius: '4px', border: '1px solid #444', backgroundColor: '#333', color: 'white', boxSizing: 'border-box' }}
                            autoFocus
                        />
                    </div>
                    <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '10px' }}>
                        <button
                            type="button"
                            onClick={() => setIsModalOpen(false)}
                            style={{ padding: '8px 16px', background: 'transparent', border: '1px solid #444', color: '#ccc', borderRadius: '4px', cursor: 'pointer' }}
                        >
                            Cancelar
                        </button>
                        <button
                            type="submit"
                            disabled={creating || !newCourseTitle.trim()}
                            style={{ padding: '8px 16px', backgroundColor: '#646cff', border: 'none', color: 'white', borderRadius: '4px', cursor: creating ? 'not-allowed' : 'pointer', opacity: creating ? 0.7 : 1 }}
                        >
                            {creating ? 'Creando...' : 'Crear Curso'}
                        </button>
                    </div>
                </form>
            </Modal>
        </div>
    );
}
export default Dashboard;
