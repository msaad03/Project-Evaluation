import React, { memo, useState } from 'react';
import { TouchableOpacity, StyleSheet, Text, View, AsyncStorage } from 'react-native';
import Background from '../components/Background';
import Logo from '../components/Logo';
import Header from '../components/Header';
import Button from '../components/Button';
import TextInput from '../components/TextInput';
import BackButton from '../components/BackButton';
import { theme } from '../core/theme';
import axios from 'axios';

import { start as startNetworkDebugging } from 'react-native-network-proxy';

import { emailValidator, passwordValidator } from '../core/utils';

const LoginScreen = ({ navigation }) => {
  const [email, setEmail] = useState({ value: '', error: '' });
  const [password, setPassword] = useState({ value: '', error: '' });
  const [errorData, setErrorData] = useState('');

  

  const _onLoginPressed = () => {

    // startNetworkDebugging({
    //   host: 'localhost',
    //   port: 59782,
    // });
    const emailError = emailValidator(email.value);
    const passwordError = passwordValidator(password.value);

    if (emailError || passwordError) {
      setEmail({ ...email, error: emailError });
      setPassword({ ...password, error: passwordError });
      return;
    }

   // navigation.navigate('Dashboard');

    axios.post('http://192.168.1.2:80/api/student/Login',  { Student_id :  email.value, Password: password.value } )
    .then( async (response) => {
      if(response.data != null)
      {
        if(response.data[0] != null)
        {
          try {
            await AsyncStorage.setItem('Group_id', response.data[0].Group_id.toString())
            await AsyncStorage.setItem('User', response.data[0].Student_id);
            
            navigation.navigate('Dashboard');
         
          } catch (error) {
            console.log(error)
          }
        }
        
          
      }
      
    })
    .catch(function (error) {
      if (error.response) {
        // Request made and server responde
        console.log(error.response.data.Message);
      } else if (error.request) {
        console.log(error.request);
        navigation.navigate('Dashboard');
        // The request was made but no response was received
        
      } else {
        // Something happened in setting up the request that triggered an Error
        console.log('Error', error.message);
      }
  
    });

    
  };

  return (
    <Background>
      <BackButton goBack={() => navigation.navigate('HomeScreen')} />
      <Header>Welcome back.</Header>
      {errorData ? <Text style={styles.error}>{errorData}</Text> : null}
      <TextInput
        label="ID"
        returnKeyType="next"
        value={email.value}
        onChangeText={text => setEmail({ value: text, error: '' })}
        error={!!email.error}
        errorText={email.error}
        autoCapitalize="none"
        autoCompleteType="email"
        textContentType="emailAddress"
        keyboardType="email-address"
      />

      <TextInput
        label="Password"
        returnKeyType="done"
        value={password.value}
        onChangeText={text => setPassword({ value: text, error: '' })}
        error={!!password.error}
        errorText={password.error}
        secureTextEntry
      />

      <View style={styles.forgotPassword}>
        <TouchableOpacity
          onPress={() => navigation.navigate('ForgotPasswordScreen')}
        >
          <Text style={styles.label}>Forgot your password?</Text>
        </TouchableOpacity>
      </View>

      <Button mode="contained" onPress={_onLoginPressed}>
        Login
      </Button>

      <View style={styles.row}>
        <Text style={styles.label}>Don’t have an account? </Text>
        <TouchableOpacity onPress={() => navigation.navigate('RegisterScreen')}>
          <Text style={styles.link}>Sign up</Text>
        </TouchableOpacity>
      </View>
    </Background>
  );
};

const styles = StyleSheet.create({
 
  forgotPassword: {
    width: '100%',
    alignItems: 'flex-end',
    marginBottom: 24,
  },
  row: {
    flexDirection: 'row',
    marginTop: 4,
  },
  label: {
    color: theme.colors.secondary,
  },
  link: {
    fontWeight: 'bold',
    color: theme.colors.primary,
  },
  error: {
    fontSize: 20,
    color: theme.colors.error,
    paddingHorizontal: 4,
    paddingTop: 4,
  },
});

export default memo(LoginScreen);
