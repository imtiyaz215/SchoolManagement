import { useQuery } from '@tanstack/react-query';
import { api } from '../lib/api';

export default function Behaviour() {
  const { data: templates } = useQuery({ queryKey: ['behaviour-templates'], queryFn: () => api.get('/behaviour/templates').then((r) => r.data) });

  return (
    <div style={{ padding: 24 }}>
      <h1>Behaviour</h1>
      <h3>Templates</h3>
      <ul>{(templates ?? []).map((t: { id: string; name: string }) => <li key={t.id}>{t.name}</li>)}</ul>
      <p style={{ color: '#888' }}>Create behaviour templates via POST /behaviour/templates</p>
    </div>
  );
}
