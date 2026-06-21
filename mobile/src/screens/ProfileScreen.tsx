import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import { useAuth } from '../auth/AuthContext';

export default function ProfileScreen() {
  const { user, logout } = useAuth();
  return (
    <View style={styles.container}>
      <Text style={styles.label}>Name</Text>
      <Text style={styles.value}>{user?.fullName}</Text>
      <Text style={styles.label}>Email</Text>
      <Text style={styles.value}>{user?.email}</Text>
      <Text style={styles.label}>Role</Text>
      <Text style={styles.value}>{user?.role}</Text>
      <TouchableOpacity style={styles.button} onPress={logout}>
        <Text style={styles.buttonText}>Logout</Text>
      </TouchableOpacity>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, padding: 24, backgroundColor: '#fff' },
  label: { color: '#888', marginTop: 12 },
  value: { fontSize: 16, marginTop: 2 },
  button: { backgroundColor: '#ff4d4f', padding: 14, borderRadius: 6, alignItems: 'center', marginTop: 32 },
  buttonText: { color: '#fff', fontWeight: '600' },
});
