import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../services/api';
import { ArrowLeft, Save, Plus, Trash2, ArrowUp, ArrowDown } from 'lucide-react';
import Modal from '../components/Modal';

interface Lesson {
    id: string;
    title: string;
    order: number;
}

interface Course {
    id: string;
    title: string;
    status: number;
}

const CourseEditor: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [course, setCourse] = useState<Course | null>(null);
    const [lessons, setLessons] = useState<Lesson[]>([]);
    const [loading, setLoading] = useState(true);
    const [title, setTitle] = useState('');

    const [isLessonModalOpen, setIsLessonModalOpen] = useState(false);
    const [newLessonTitle, setNewLessonTitle] = useState('');
    const [creatingLesson, setCreatingLesson] = useState(false);

    const fetchData = async () => {
        setLoading(true);
        try {
            const [courseRes, lessonsRes] = await Promise.all([
                api.get(`/courses/${id}`),
                api.get(`/courses/${id}/lessons`)
            ]);
            setCourse(courseRes.data);
            setTitle(courseRes.data.title);
            setLessons(lessonsRes.data);
        } catch (error) {
            console.error('Error', error);
            alert('Error cargando curso');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (id) fetchData();
    }, [id]);

    const handleSaveCourse = async () => {
        try {
            await api.put(`/courses/${id}`, { title });
            alert('Curso actualizado');
            fetchData();
        } catch (error) {
            alert('Error actualizando curso');
        }
    };

    const handlePublish = async () => {
        try {
            if (course?.status === 0) {
                await api.patch(`/courses/${id}/publish`);
            } else {
                await api.patch(`/courses/${id}/unpublish`);
            }
            fetchData();
        } catch (err: any) {
            // Backend middleware returns { "message": "..." }
            const msg = err.response?.data?.message || 'Error cambiando estado';
            alert(msg);
        }
    }

    const handleAddLessonSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!newLessonTitle.trim()) return;

        setCreatingLesson(true);
        try {
            // Order: last + 1
            const maxOrder = lessons.length > 0 ? Math.max(...lessons.map(l => l.order)) : 0;
            await api.post('/lessons', { courseId: id, title: newLessonTitle, order: maxOrder + 1 });
            setNewLessonTitle('');
            setIsLessonModalOpen(false);
            fetchData();
        } catch (error) {
            alert('Error creando lección');
        } finally {
            setCreatingLesson(false);
        }
    };

    const handleDeleteLesson = async (lessonId: string) => {
        if (!confirm('Eliminar lección?')) return;
        try {
            await api.delete(`/lessons/${lessonId}`);
            fetchData();
        } catch (error) {
            alert('Error eliminando lección');
        }
    };

    const handleReorder = async (lessonId: string, newOrder: number) => {
        try {
            await api.patch(`/lessons/${lessonId}/reorder`, { newOrder });
            fetchData();
        } catch (error) {
            alert('Error reordenando');
        }
    };

    if (loading) return <div>Cargando...</div>;
    if (!course) return <div>Curso no encontrado</div>;

    return (
        <div>
            <button onClick={() => navigate('/dashboard')} style={{ background: 'none', border: 'none', color: '#888', cursor: 'pointer', display: 'flex', alignItems: 'center', marginBottom: '20px' }}>
                <ArrowLeft size={16} style={{ marginRight: '5px' }} /> Volver
            </button>

            <div style={{ backgroundColor: '#2a2a2a', padding: '20px', borderRadius: '8px', marginBottom: '30px' }}>
                <h2 style={{ marginTop: 0, color: '#646cff' }}>Editar Curso</h2>
                <div style={{ display: 'flex', gap: '20px', alignItems: 'end' }}>
                    <div style={{ flex: 1 }}>
                        <label style={{ display: 'block', marginBottom: '5px', color: '#ccc' }}>Título</label>
                        <input
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            style={{ width: '100%', padding: '10px', borderRadius: '4px', border: '1px solid #444', backgroundColor: '#333', color: 'white' }}
                        />
                    </div>
                    <button onClick={handleSaveCourse} style={{ padding: '10px', backgroundColor: '#2a2a2a', border: '1px solid #646cff', color: '#646cff', borderRadius: '4px', cursor: 'pointer' }}>
                        <Save size={18} />
                    </button>
                    <button
                        onClick={handlePublish}
                        style={{ padding: '10px 20px', backgroundColor: course.status === 0 ? '#2ecc71' : '#f1c40f', border: 'none', color: 'black', borderRadius: '4px', cursor: 'pointer', fontWeight: 'bold' }}
                    >
                        {course.status === 0 ? 'Publicar' : 'Despublicar'}
                    </button>
                </div>
            </div>

            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
                <h3>Lecciones</h3>
                <button
                    onClick={() => setIsLessonModalOpen(true)}
                    style={{ display: 'flex', alignItems: 'center', gap: '5px', padding: '8px 12px', backgroundColor: '#646cff', border: 'none', borderRadius: '4px', color: 'white', cursor: 'pointer' }}
                >
                    <Plus size={16} /> Agregar Lección
                </button>
            </div>

            <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                {lessons.sort((a, b) => a.order - b.order).map((lesson, index) => (
                    <div key={lesson.id} style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', backgroundColor: '#2a2a2a', padding: '15px', borderRadius: '4px', border: '1px solid #333' }}>
                        <div style={{ display: 'flex', alignItems: 'center', gap: '15px' }}>
                            <span style={{ fontWeight: 'bold', color: '#666' }}>#{lesson.order}</span>
                            <span>{lesson.title}</span>
                        </div>
                        <div style={{ display: 'flex', gap: '10px' }}>
                            {index > 0 && <button onClick={() => handleReorder(lesson.id, lessons[index - 1].order)} style={{ background: 'none', border: 'none', color: '#ccc', cursor: 'pointer' }}><ArrowUp size={16} /></button>}
                            {index < lessons.length - 1 && <button onClick={() => handleReorder(lesson.id, lessons[index + 1].order)} style={{ background: 'none', border: 'none', color: '#ccc', cursor: 'pointer' }}><ArrowDown size={16} /></button>}
                            <button onClick={() => handleDeleteLesson(lesson.id)} style={{ background: 'none', border: 'none', color: '#ff6b6b', cursor: 'pointer' }}><Trash2 size={16} /></button>
                        </div>
                    </div>
                ))}
                {lessons.length === 0 && <p style={{ color: '#888', fontStyle: 'italic' }}>No hay lecciones. Agrega una para comenzar.</p>}
            </div>

            <Modal
                isOpen={isLessonModalOpen}
                onClose={() => setIsLessonModalOpen(false)}
                title="Nueva Lección"
            >
                <form onSubmit={handleAddLessonSubmit}>
                    <div style={{ marginBottom: '20px' }}>
                        <label style={{ display: 'block', marginBottom: '8px', color: '#ccc' }}>Título de la Lección</label>
                        <input
                            type="text"
                            value={newLessonTitle}
                            onChange={(e) => setNewLessonTitle(e.target.value)}
                            placeholder="Ej. Configuración del entorno"
                            style={{ width: '100%', padding: '10px', borderRadius: '4px', border: '1px solid #444', backgroundColor: '#333', color: 'white', boxSizing: 'border-box' }}
                            autoFocus
                        />
                    </div>
                    <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '10px' }}>
                        <button
                            type="button"
                            onClick={() => setIsLessonModalOpen(false)}
                            style={{ padding: '8px 16px', background: 'transparent', border: '1px solid #444', color: '#ccc', borderRadius: '4px', cursor: 'pointer' }}
                        >
                            Cancelar
                        </button>
                        <button
                            type="submit"
                            disabled={creatingLesson || !newLessonTitle.trim()}
                            style={{ padding: '8px 16px', backgroundColor: '#646cff', border: 'none', color: 'white', borderRadius: '4px', cursor: creatingLesson ? 'not-allowed' : 'pointer', opacity: creatingLesson ? 0.7 : 1 }}
                        >
                            {creatingLesson ? 'Creando...' : 'Crear Lección'}
                        </button>
                    </div>
                </form>
            </Modal>
        </div>
    );
};

export default CourseEditor;
