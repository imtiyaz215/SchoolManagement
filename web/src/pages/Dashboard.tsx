import { useQuery } from '@tanstack/react-query';
import { studentService } from '../services/students';
import { referenceService } from '../services/reference';

export default function Dashboard() {
  const { data: students } = useQuery({ queryKey: ['students', 'count'], queryFn: () => studentService.list({ pageSize: 1 }) });
  const { data: sessions } = useQuery({ queryKey: ['sessions'], queryFn: referenceService.sessions });
  const { data: classes } = useQuery({ queryKey: ['classes'], queryFn: referenceService.classes });

  return (
    <div style={{ padding: 24 }}>
      <h1>Dashboard</h1>
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4, 1fr)', gap: 16, marginTop: 24 }}>
        <Card title="Total Students" value={students?.total ?? '...'} />
        <Card title="Active Sessions" value={sessions?.length ?? '...'} />
        <Card title="Classes" value={classes?.length ?? '...'} />
        <Card title="Behaviour Pending" value="-" />
      </div>
    </div>
  );
}

function Card({ title, value }: { title: string; value: string | number }) {
  return (
    <div style={{ background: 'white', padding: 20, borderRadius: 8, border: '1px solid #eee' }}>
      <div style={{ color: '#888', fontSize: 13 }}>{title}</div>
      <div style={{ fontSize: 28, fontWeight: 600, marginTop: 8 }}>{value}</div>
    </div>
  );
}
