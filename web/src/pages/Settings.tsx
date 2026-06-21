import { useQuery } from '@tanstack/react-query';
import { referenceService } from '../services/reference';

export default function Settings() {
  const religions = useQuery({ queryKey: ['religions'], queryFn: referenceService.religions });
  const houses = useQuery({ queryKey: ['houses'], queryFn: referenceService.houses });
  const states = useQuery({ queryKey: ['states'], queryFn: referenceService.states });

  return (
    <div style={{ padding: 24 }}>
      <h1>Reference Data</h1>
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: 16 }}>
        <Card title="Religions" items={religions.data} />
        <Card title="Houses" items={houses.data} />
        <Card title="States" items={states.data} />
      </div>
    </div>
  );
}

function Card({ title, items }: { title: string; items: { id: string; name: string }[] | undefined }) {
  return (
    <div style={{ background: 'white', padding: 16, borderRadius: 8, border: '1px solid #eee' }}>
      <h4>{title}</h4>
      <ul>{(items ?? []).map((i) => <li key={i.id}>{i.name}</li>)}</ul>
    </div>
  );
}
