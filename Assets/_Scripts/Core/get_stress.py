#! C:\Users\Rawan\Anaconda2\envs\tensorflow_gpu
import sys
import os
import random
import numpy as np
from keras.models import model_from_json

#tensorflow_path = os.environ["TENSOR_PATH"]
tensorflow_path = os.getenv("TENSOR_PATH","c:\\")#os.environ["TENSOR_PATH"]
sys.path = [tensorflow_path,
tensorflow_path + '\\python35.zip', 
tensorflow_path + '\\DLLs', 
tensorflow_path + '\\lib', 
tensorflow_path + '\\lib\\site-packages',
tensorflow_path + '\\Lib\\site-packages']
os.environ["PYTHONPATH"] = tensorflow_path + '\\python'


#look_back = 250
look_back = 350
data_size = 10
#model_name = '01_31_20_12'
model_name = '02_15_17_53'
#model_name = '02_16_21_56'

weight_path = 'MODELS\\%s_weights.best.hdf5'%(model_name)
model_path = 'MODELS\\%s_model.json' %(model_name)

def load_model():
	with open(model_path, 'r') as model_file:
		model_json = model_file.read()

	model = model_from_json(model_json)
	model.load_weights(weight_path)
	print('loaded model from disk')
	model.compile(loss='binary_crossentropy', optimizer='adam', metrics=['accuracy'])

	return model

def test_stress(arg):
	arg = arg[0].split(',')
	arg = [float(i) for i in arg]
	print(arg)
	return random.random()

def compute_stress(arg):
	#format data into numpy 
#	arg = arg[0].split(',')
	arg = arg.split(',')
	arg = [float(i) for i in arg]
	arg = np.array(arg)
	X = arg
	del arg
	X = np.reshape(X, (1,look_back,1))
	print(X.shape)
	model = load_model()
	score = model.predict(X)
	return score[0]

def compute_stress_model(arg,model):
#	print(arg)
	arg = arg.split(',')
	arg = [float(i) for i in arg]
	arg = np.array(arg)
	X = arg
	del arg
	X = np.reshape(X, (1, look_back, 1))
#	print(X.shape)
#	model = load_model()
	score = model.predict(X)
	return score[0]

#prnt(compute_stress(sys.argv[1:]))
#st = ""
#for i in range(350):
#	st = st + str(random.random()) + ","
#print(st)
#st = st[:-1]
#print(compute_stress(st))