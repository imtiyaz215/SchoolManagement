import { View, Text, StyleSheet, FlatList } from 'react-native';
import { useQuery } from '@tanstack/react-query';
import { api } from '../api';

interface BehaviourItem { id: string; name: string; }

export default function BehaviourScreen() {
  const { data: templates } = useQuery({ queryKey: ['behaviour-templates'], queryFn: async () => (await api.get<{ id: string; name: string; items: BehaviourItem[] }[]>('/behaviour/templates')).data });
  const { data: sheets } = useQuery({ queryKey: ['behaviour-history'], queryFn: async () => (await api.get('/mobile/parent/behaviour')).data });

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Behaviour</Text>
      <Text style={styles.subtitle}>Templates</Text>
      {(templates ?? []).map((t) => (
        <View key={t.id} style={styles.card}>
          <Text style={styles.name}>{t.name}</Text>
          <Text style={styles.meta}>{t.items?.length ?? 0} items</Text>
        </View>
      ))}
      <Text style={styles.subtitle}>My Submissions</Text>
      <FlatList
        data={Array.isArray(sheets) ? sheets : []}
        keyExtractor={(s: { id: string }) => s.id}
        ListEmptyComponent={<Text style={styles.empty}>No submissions yet.</Text>}
        renderItem={({ item }) => (
          <View style={styles.card}>
            <Text style={styles.name}>{String((item as { month: number }).month)}/{String((item as { year: number }).year)}</Text>
            <Text style={styles.meta}>{String((item as { status: string }).status)}</Text>
          </View>
        )}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, padding: 16, backgroundColor: '#fff' },
  title: { fontSize: 20, fontWeight: '600', marginBottom: 12 },
  subtitle: { fontSize: 16, fontWeight: '600', marginTop: 12, marginBottom: 8 },
  card: { padding: 12, borderWidth: 1, borderColor: '#eee', borderRadius: 6, marginBottom: 8 },
  name: { fontWeight: '600' },
  meta: { color: '#666', marginTop: 4 },
  empty: { color: '#888', textAlign: 'center', marginTop: 20 },
});
