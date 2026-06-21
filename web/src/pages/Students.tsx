import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { studentService } from '../services/students';

export default function Students() {
  const [search, setSearch] = useState('');
  const [page, setPage] = useState(1);
  const pageSize = 20;

  const { data, isLoading } = useQuery({
    queryKey: ['students', search, page],
    queryFn: () => studentService.list({ search, page, pageSize }),
  });

  return (
    <div style={{ padding: 24 }}>
      <h1>Students</h1>
      <input
        type="search"
        placeholder="Search by name or admission no."
        value={search}
        onChange={(e) => { setSearch(e.target.value); setPage(1); }}
        style={{ padding: 8, width: 320, marginBottom: 16 }}
      />
      {isLoading ? <p>Loading...</p> : (
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ borderBottom: '2px solid #eee', textAlign: 'left' }}>
              <th>Admission</th><th>Name</th><th>Gender</th><th>DOB</th><th>Status</th>
            </tr>
          </thead>
          <tbody>
            {data?.items.map((s) => (
              <tr key={s.id} style={{ borderBottom: '1px solid #f0f0f0' }}>
                <td>{s.admissionNumber}</td>
                <td>{s.fullName}</td>
                <td>{s.gender}</td>
                <td>{s.dob}</td>
                <td>{s.status}</td>
              </tr>
            ))}
            {data?.items.length === 0 && (
              <tr><td colSpan={5} style={{ padding: 24, textAlign: 'center', color: '#888' }}>No students yet. Seed data creates a demo school; add students via API.</td></tr>
            )}
          </tbody>
        </table>
      )}
      <div style={{ marginTop: 16, display: 'flex', gap: 8, alignItems: 'center' }}>
        <button disabled={page <= 1} onClick={() => setPage(page - 1)}>Prev</button>
        <span>Page {page} of {Math.max(1, Math.ceil((data?.total ?? 0) / pageSize))}</span>
        <button disabled={page * pageSize >= (data?.total ?? 0)} onClick={() => setPage(page + 1)}>Next</button>
      </div>
    </div>
  );
}
