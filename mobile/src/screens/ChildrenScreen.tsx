import { View, Text, StyleSheet, FlatList } from 'react-native';
import { useQuery } from '@tanstack/react-query';
import { api } from '../api';

interface Child { id: string; fullName: string; admissionNumber: string; className: string; section: string; }

export default function ChildrenScreen() {
  const { data, isLoading } = useQuery({ queryKey: ['children'], queryFn: async () => (await api.get<{ items: Child[] }>('/mobile/parent/children')).data.items });

  return (
    <View style={styles.container}>
      <Text style={styles.title}>My Children</Text>
      {isLoading ? <Text>Loading...</Text> : (
        <FlatList
          data={data ?? []}
          keyExtractor={(c) => c.id}
          ListEmptyComponent={<Text style={styles.empty}>No children linked yet. Contact school admin.</Text>}
          renderItem={({ item }) => (
            <View style={styles.row}>
              <Text style={styles.name}>{item.fullName}</Text>
              <Text style={styles.meta}>{item.className} - {item.section} - {item.admissionNumber}</Text>
            </View>
          )}
        />
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, padding: 16, backgroundColor: '#fff' },
  title: { fontSize: 20, fontWeight: '600', marginBottom: 12 },
  row: { padding: 16, borderWidth: 1, borderColor: '#eee', borderRadius: 6, marginBottom: 8 },
  name: { fontWeight: '600', fontSize: 16 },
  meta: { color: '#666', marginTop: 4 },
  empty: { color: '#888', textAlign: 'center', marginTop: 40 },
});
