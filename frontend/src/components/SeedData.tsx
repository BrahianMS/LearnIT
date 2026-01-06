import React, { useState } from 'react';
import api from '../services/api';

const SeedData: React.FC = () => {
    const [seeding, setSeeding] = useState(false);

    const seedCourses = async () => {
        if (!confirm('Esto creará los cursos iniciales. ¿Continuar?')) return;
        setSeeding(true);

        const coursesToCreate = [
            {
                title: 'Introducción a .NET 8',
                lessons: [
                    { title: 'Configuración de entorno', order: 1 },
                    { title: 'Estructura de proyecto', order: 2 },
                    { title: 'Controladores y Rutas', order: 3 }
                ]
            },
            {
                title: 'React desde Cero',
                lessons: [
                    { title: 'Componentes y Props', order: 1 },
                    { title: 'Hooks Básicos', order: 2 },
                    { title: 'React Router', order: 3 }
                ]
            },
            {
                title: 'Clean Architecture',
                lessons: [
                    { title: 'Dominio y Reglas de Negocio', order: 1 },
                    { title: 'Capa de Aplicación', order: 2 },
                    { title: 'Infraestructura', order: 3 }
                ]
            }
        ];

        try {
            for (const courseData of coursesToCreate) {
                // 0. Check if exists
                const searchRes = await api.get('/courses', { params: { searchTerm: courseData.title } });
                const existing = searchRes.data.items?.find((c: any) => c.title === courseData.title);

                if (existing) {
                    console.log(`Curso "${courseData.title}" ya existe. Omitiendo.`);
                    continue;
                }

                // 1. Create Course
                const courseRes = await api.post('/courses', { title: courseData.title });
                const courseId = courseRes.data.id; // Assuming API returns object with Id or the object itself

                // 2. Add Lessons
                for (const lesson of courseData.lessons) {
                    await api.post('/lessons', { ...lesson, courseId });
                }

                // 3. Publish
                await api.patch(`/courses/${courseId}/publish`);
            }
            alert('Cursos verificados/creados exitosamente!');
            window.location.reload();
        } catch (error) {
            console.error(error);
            alert('Error durante la carga de datos.');
        } finally {
            setSeeding(false);
        }
    };

    return (
        <button
            onClick={seedCourses}
            disabled={seeding}
            style={{
                position: 'fixed', bottom: '20px', right: '20px',
                backgroundColor: '#f1c40f', color: 'black', border: 'none',
                padding: '10px 20px', borderRadius: '50px', cursor: 'pointer',
                fontWeight: 'bold', boxShadow: '0 4px 10px rgba(0,0,0,0.3)'
            }}
        >
            {seeding ? 'Cargando...' : '⚡ Cargar Datos Demo'}
        </button>
    );
};

export default SeedData;
