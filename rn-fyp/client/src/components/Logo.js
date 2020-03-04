import React, { memo } from 'react';
import { Image, StyleSheet, Text } from 'react-native';

const Logo = () => (
  <Text style={styles.style}> <Text style={styles.innerStyle}>FYP</Text> Evaluation</Text>
);

const styles = StyleSheet.create({
  innerStyle: {
    marginLeft: 60,
  },

  style: {
    fontSize: 50,
    fontWeight: 'bold',
    color: '#2095F2',
    marginBottom: 12,
    marginLeft: 35,
  },
});

export default memo(Logo);
