import { useQuery } from '@tanstack/react-query';
import { referenceService } from '../services/reference';

export default function Academic() {
  const { data: sessions } = useQuery({ queryKey: ['sessions'], queryFn: referenceService.sessions });
  const { data: classGroups } = useQuery({ queryKey: ['classGroups'], queryFn: referenceService.classGroups });
  const { data: classes } = useQuery({ queryKey: ['classes'], queryFn: referenceService.classes });

  return (
    <div style={{ padding: 24 }}>
      <h1>Academic</h1>
      <Section title="Academic Sessions">
        <ul>{(sessions ?? []).map((s: { id: string; name: string; isActive: boolean }) => (
          <li key={s.id}>{s.name} {s.isActive && <b>(Active)</b>}</li>
        ))}</ul>
      </Section>
      <Section title="Class Groups">
        <ul>{(classGroups ?? []).map((g: { id: string; name: string }) => <li key={g.id}>{g.name}</li>)}</ul>
      </Section>
      <Section title="Classes">
        <ul>{(classes ?? []).map((c: { id: string; name: string }) => <li key={c.id}>{c.name}</li>)}</ul>
      </Section>
    </div>
  );
}

function Section({ title, children }: { title: string; children: React.ReactNode }) {
  return (
    <div style={{ marginBottom: 24 }}>
      <h3>{title}</h3>
      {children}
    </div>
  );
}
