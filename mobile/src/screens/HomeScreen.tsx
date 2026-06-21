import { View, Text, StyleSheet } from 'react-native';
import { useAuth } from '../auth/AuthContext';

export default function HomeScreen() {
  const { user } = useAuth();
  return (
    <View style={styles.container}>
      <Text style={styles.greeting}>Welcome,</Text>
      <Text style={styles.name}>{user?.fullName}</Text>
      <Text style={styles.subtitle}>View your children's progress and submit monthly behaviour reports.</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, padding: 24, backgroundColor: '#fff' },
  greeting: { fontSize: 16, color: '#666' },
  name: { fontSize: 24, fontWeight: '600', marginTop: 4, marginBottom: 16 },
  subtitle: { color: '#666', lineHeight: 22 },
});
