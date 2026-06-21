import { View, Text, StyleSheet } from 'react-native';
import { useRoute, type RouteProp } from '@react-navigation/native';

export default function ChildDetailScreen() {
  const route = useRoute<RouteProp<{ ChildDetail: { id: string } }>>();
  return (
    <View style={styles.container}>
      <Text style={styles.title}>Child Detail</Text>
      <Text>ID: {route.params?.id}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, padding: 16, backgroundColor: '#fff' },
  title: { fontSize: 20, fontWeight: '600', marginBottom: 12 },
});
